﻿namespace Platform.Net
{
    /// <summary>
    /// 消息编号
    /// </summary>
    public enum MsgNoS2C
    {
        //启动 登录 消息
        RESPONSE_LOGIN_S2C = 1001,                      //登陆
        //大厅协议
        RESPONSE_CREATEROOM_S2C = 2002,                 //创建房间
        RESPONSE_GETCARDINFO_S2C = 2003,                //获取道具
        RESPONSE_GETSIGNININFO_S2C = 2004,              //获取签到信息
        RESPONSE_USERSIGNIN_S2C = 2005,                 //玩家签到
        RESPONSE_GETUSERINFO_S2C = 2006,                //大厅登陆,获取玩家信息
        RESPONSE_GETCHECKINREWARD_S2C = 2008,           //签到领取
        RESPONSE_JOINROOM_S2C = 2009,                   //创建房间
        RESPONSE_GETGRADEINFO_S2C = 2010,               //获取战绩信息
        RESPONSE_GETROUNDINFO_S2C = 2011,               //获取具体房间对战信息
        RESPONSE_CHECKAPPLYSTATE_S2C = 2012,            //检查比赛状态
        RESPONSE_APPLYCOMPETITION_S2C = 2013,           //报名比赛
        RESPONSE_JOINCOMPETITION_S2C = 2014,            //参赛
        RESPONSE_CHECKINVITATIONCODE_S2C = 2015,        //验证邀请码
        RESPONSE_PUSHANNOUNCEMENT_S2C = 2016,           //公告
        RESPONSE_APPLYMATCHING_S2C = 2017,              //申请匹配
        RESPONSE_MATCHINGSUCCEED_S2C = 2018,            //匹配成功
        RESPONSE_CANCELMATCHING_S2C = 2019,             //取消匹配
        RESPONSE_GetRankingListC2S = 2020,                //获取排行榜信息
        RESPONSE_NOTICECONFIG_S2C = 2021,               //获取公告
        GET_PLAYERINFO_S2C = 2022,                      //获取玩家信息
        RESPONSE_PLAYVIDEO_S2C = 2023,                  //回放
        RECONNECT_S2C = 2024,                           //断线重连
        RESPONSE_REFRESHUSERITEM_S2C = 2025,                    //更新玩家货币
        HALL_BEAT_S2C = 2026,//房间心跳包

        //平台消息
        ENTER_ROOMSERVER_S2C = 4000,                    //进入房间服务器
        JOIN_ROOM_S2C = 4001,//加入房间

        //战斗相关消息
        PUSH_READY = 4002,//推送准备
        PUSH_JOIN = 4003,//推送加入房间
        DISSOLUTION_S2C = 4004,//解散房间
        EXIT_S2C = 4005,//离开房间
        DISLOVEAPPLY_S2C = 4006,//申请解散返回
        CANCEL_DISSLOVEAPPLY_S2C = 4007,//取消申请解散
        DISSLOVEROOM_CONFIRM_S2C = 4008,//房间解散二次确认
        GAME_START_S2C = 4010,//推送发牌
        PLAYAMAHJONG_S2C = 4011,//出牌
        PLAYAFLOWERMAHJONG_S2C = 4017,//打花牌
        GUO_S2C = 4012,//过
        PENG_S2C = 4013,//碰
        CHI_S2C = 4014,
        PUSH_PLAYER_ACT = 4015,//推送角色动作
        PUSH_PLAYER_ACTTIP = 4016,//推送角色动作提示
        COMMONANGANG_S2C = 4020,//普通暗杠
        BACKANGANG_S2C = 4021,//回头暗杠
        ZHIGANG_S2C = 4022,//直杠
        COMMONPENGGANG_S2C = 4023,//普通碰杠
        BACKPENGGANG_S2C = 4024,//回头碰杠
        ZIMOHU_S2C = 4030,//自摸
        QIANGZHIGANGHU_S2C = 4032,//抢直杠胡
        QIANGANGANGHU_S2C = 4033,//抢直杠胡
        QIANGPENGGANGHU_S2C = 4034,//抢直杠胡
        CHIHU_S2C = 4035,//抢直杠胡
        PUSH_MATCH_END = 4040,//推送本局结束
        PUSH_ROOM_END = 4041,//推送本房间结束
        ACTERROR_S2C = 4050,//出牌出错
        BATTLE_BEAT_S2C = 4051,//房间心跳包
        PUSH_CHAT = 4060,//聊天推送
        PUSH_VOICE = 4061,//语言推送


        PLAY_REPORT_S2C = 9908,//播放战报返回
    }

    /// <summary>
    /// 消息编号
    /// </summary>
    public enum MsgNoC2S
    {
        //启动 登录 消息
        REQUEST_LOGIN_C2S = 1001,                       //登陆
        //大厅协议
        REQUEST_CREATEROOM_C2S = 2002,                  //创建房间
        REQUEST_GETCARDINFO_C2S = 2003,                 //获取道具
        REQUEST_GETSIGNININFO_C2S = 2004,               //获取签到信息
        REQUEST_USERSIGNIN_C2S = 2005,                  //玩家签到
        REQUEST_GETUSERINFO_C2S = 2006,                 //大厅登陆,请求玩家信息
        REQUEST_GETCHECKINREWARD_C2S = 2008,            //签到领取
        REQUEST_JOINROOM_C2S = 2009,                    //创建房间
        REQUEST_GETGRADEINFO_C2S = 2010,                //获取战绩信息
        REQUEST_GETROUNDINFOC_C2S = 2011,               //获取具体房间对战信息
        REQUEST_CHECKAPPLYSTATE_C2S = 2012,             //检查比赛状态
        REQUEST_APPLYCOMPETITION_C2S = 2013,            //报名比赛
        REQUEST_JOINCOMPETITION_C2S = 2014,             //参赛
        REQUEST_CHECKINVITATIONCODE_C2S = 2015,         //验证邀请码
        REQUEST_APPLYMATCHING_C2S = 2017,               //申请匹配
        REQUEST_CANCELMATCHING_C2S = 2019,             //取消匹配
        REQUEST_GetRankingListC2S =2020,                //获取排行榜信息
        GET_PLAYERINFO_C2S = 2022,//获取玩家信息
        REQUEST_PLAYVIDEO_C2S = 2023,                   //回放
        RECONNECT_C2S = 2024,                           //断线重连
        HALL_BEAT_C2S = 2026,                           //房间心跳包

        //平台消息
        ENTER_ROOMSERVER_C2S = 4000,                    //进入房间服务器
        JOIN_ROOM_C2S = 4001,//加入房间

        //战斗相关消息
        READY_C2S = 4002,//准备
        DISSOLUTION_C2S = 4004,//解散房间
        EXIT_C2S = 4005,//离开房间
        DISLOVEAPPLY_C2S = 4006,//申请解散房间
        CANCEL_DISSLOVEAPPLY_C2S = 4007,//取消申请解散
        DISSLOVEROOM_CONFIRM_C2S = 4008,//房间解散二次确认
        PLAYAMAHJONG_C2S = 4011,//出牌
        PlayAFlowerMahjongC2S = 4017,//打花牌
        GUO_C2S = 4012,//过
        PENG_C2S = 4013,//碰
        CHI_C2S  = 4014,//吃
        COMMONANGANG_C2S = 4020,//普通暗杠
        ZHIGANG_C2S = 4022,//直杠(明杠)

        BACKANGANG_C2S = 4021,//回头暗杠
        COMMONPENGGANG_C2S = 4023,//普通碰杠
        BACKPENGGANG_C2S = 4024,//回头碰杠

        ZIMOHU_C2S = 4030,//自摸
        QIANGZHIGANGHU_C2S = 4032,//抢直杠胡
        QIANGANGANGHU_C2S = 4033,//抢直杠胡
        QIANGPENGGANGHU_C2S = 4034,//抢直杠胡
        CHIHU_C2S = 4035,//抢直杠胡


        BATTLE_BEAT_C2S = 4051,//房间心跳包
        SEND_VOICE_C2S = 4061,//发送语音信息
        SEND_CHAT_C2S = 4060,//发送聊天

        PLAY_REPORT_C2S = 9905,//播放战报
    }
}