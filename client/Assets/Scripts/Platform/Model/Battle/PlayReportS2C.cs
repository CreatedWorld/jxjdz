using System;
using LitJson;
using System.Collections.Generic;


namespace Platform.Model.VO.BattleVO
{
    /// <summary>
    /// 牌局战报
    /// </summary>
    public class PlayReportS2C
    {
        public PlayReportS2C() { }

        /// <summary>
        /// 牌局初始信息
        /// </summary>
        public JoinRoomS2C joinInfo;
        /// <summary>
        /// 牌局开始时间
        /// </summary>
        public long startTime;
        /// <summary>
        /// 牌局动作数组
        /// </summary>
        public List<ActionVO> actions = new List<ActionVO>();

        /// <summary>
        /// 字符串解析为战报对象
        /// </summary>
        /// <param name="reportStr"></param>
        /// <returns></returns>
        public static PlayReportS2C Paser(string reportStr)
        {
            JsonData jsonData = JsonMapper.ToObject(reportStr);
            PlayReportS2C reportS2C = new PlayReportS2C();
            reportS2C.startTime = long.Parse(jsonData["startTime"].ToString());
            reportS2C.joinInfo = new JoinRoomS2C();
            var joinInfo = jsonData["joinInfo"];
            reportS2C.joinInfo.innings = int.Parse(joinInfo["innings"].ToString());
            reportS2C.joinInfo.createId = int.Parse(joinInfo["createId"].ToString());
            reportS2C.joinInfo.curInnings = int.Parse(joinInfo["curInnings"].ToString());
            reportS2C.joinInfo.isStart = bool.Parse(joinInfo["isStart"].ToString());
            if (joinInfo.Inst_Object.ContainsKey("leftCardCount"))
            {
                //reportS2C.joinInfo.leftCardCount = int.Parse(joinInfo["leftCardCount"].ToString());
                reportS2C.joinInfo.leftCardCount = 93;
            }
            else
            {
                reportS2C.joinInfo.leftCardCount = GlobalData.CardWare.Length - 53;
            }
            for (int i = 0; i < joinInfo["playInfoArr"].Count; i++)
            {
                var playerInfoJson = joinInfo["playInfoArr"][i];
                var playerInfoVO = new PlayerInfoVOS2C();
                try
                {
                    playerInfoVO.getCard = int.Parse(playerInfoJson["getCard"].ToString());
                }
                catch
                {
                    
                }
                playerInfoVO.headIcon = playerInfoJson["headIcon"].ToString();
                playerInfoVO.isBanker = bool.Parse(playerInfoJson["isBanker"].ToString());
                playerInfoVO.isMaster = bool.Parse(playerInfoJson["isMaster"].ToString());
                playerInfoVO.isReady = bool.Parse(playerInfoJson["isReady"].ToString());
                playerInfoVO.name = playerInfoJson["name"].ToString();
                playerInfoVO.score = int.Parse(playerInfoJson["score"].ToString());
                playerInfoVO.sex = int.Parse(playerInfoJson["sex"].ToString());
                playerInfoVO.sit = int.Parse(playerInfoJson["sit"].ToString());
                playerInfoVO.userId = int.Parse(playerInfoJson["userId"].ToString());
                for (int j = 0; j < playerInfoJson["handCards"].Count; j++)
                {
                    playerInfoVO.handCards.Add(int.Parse(playerInfoJson["handCards"][j].ToString()));
                }
                reportS2C.joinInfo.playInfoArr.Add(playerInfoVO);
            }

            var actTip = paserActTip(joinInfo["playerTipAct"]);
            reportS2C.joinInfo.playerTipAct = actTip;
            reportS2C.joinInfo.roomCode = joinInfo["roomCode"].ToString();
            var actionsJson = jsonData["actions"];
      
            long perActTime = 0;
            for (int i = 0; i < actionsJson.Count; i++)
            {
                var actionJson = actionsJson[(i + 1).ToString()];
                var actionVO = new ActionVO();
                actionVO.isActionTip = bool.Parse(actionJson["isActionTip"].ToString());
                actionVO.actionTime = long.Parse(actionJson["actionTime"].ToString());
                if (i == 0 && actionVO.actionTime - reportS2C.startTime > 10000)//判断第一步距离开始事件是否超过10秒
                {
                    actionVO.actionTime = reportS2C.startTime + 3000;
                }
                if (perActTime == 0)
                {
                    perActTime = actionVO.actionTime;
                }
                if (actionVO.actionTime - perActTime > 10000)
                {
                    actionVO.actionTime = perActTime + 3000;
                }
                perActTime = actionVO.actionTime;
                if (actionVO.isActionTip)
                {
                    actionVO.actTip = paserActTip(actionJson["actTip"]);
                    actionVO.actTip.tipRemainUT = actionVO.actionTime;
                }
                else
                {
                    actionVO.act = paserAct(actionJson["act"]);
                 
                }
                reportS2C.actions.Add(actionVO);
            }
            return reportS2C;
        }

        /// <summary>
        /// json格式转为PushPlayerActTipS2C
        /// </summary>
        /// <param name="actTipJson"></param>
        /// <returns></returns>
        private static PushPlayerActTipS2C paserActTip(JsonData actTipJson)
        {
            if (actTipJson == null)
            {
                return null;
            }
            var actTip = new PushPlayerActTipS2C();
            try
            {
                for (int i = 0; i < actTipJson["actCards"].Count; i++)
                {
                    actTip.actCards.Add(int.Parse(actTipJson["actCards"][i].ToString()));
                }
                for (int i = 0; i < actTipJson["acts"].Count; i++)
                {
                    actTip.acts.Add((PlayerActType)int.Parse(actTipJson["acts"][i].ToString()));
                }
                actTip.optUserId = int.Parse(actTipJson["optUserId"].ToString());
                actTip.tipRemainTime = int.Parse(actTipJson["tipRemainTime"].ToString());
                actTip.tipRemainUT = long.Parse(actTipJson["tipRemainUT"].ToString());
                return actTip;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// json格式转为PushPlayerActS2C
        /// </summary>
        /// <param name="actJson"></param>
        /// <returns></returns>
        private static PushPlayerActS2C paserAct(JsonData actJson)
        {
            var act = new PushPlayerActS2C();
            act.act = (PlayerActType)int.Parse(actJson["act"].ToString());
            act.actCard = int.Parse(actJson["actCard"].ToString());
            if (actJson["chiCards"].Count>0)
            {
                for (int i = 0; i < actJson["chiCards"].Count; i++)
                {
                    act.chiCards.Add(int.Parse(actJson["chiCards"][i].ToString()));
                }
            }
            try
            {
                for (int i = 0; i < actJson["flowerCards"].Count; i++)
                {
                    act.flowerCards.Add(int.Parse(actJson["flowerCards"].ToString()));
                }
            }
            catch (Exception)
            {
                
            }
            act.targetUserId = int.Parse(actJson["targetUserId"].ToString());
            act.userId = int.Parse(actJson["userId"].ToString());
            return act;
        }

    }
}
