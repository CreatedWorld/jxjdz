﻿using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Platform.Model.Battle;
using UnityEngine.UI;

namespace Platform.Utils
{
	/// <summary>
	/// 资源管理类
	/// </summary>
	public class ResourcesMgr
	{
		/// <summary>
		/// 牌面名称和网格数据数组
		/// </summary>
		private Dictionary<string, Mesh> meshsDic = new Dictionary<string, Mesh> ();
		/// <summary>
		/// 牌面序号和牌面名称
		/// </summary>
		private Dictionary<int, string> meshNameDic = new Dictionary<int, string> ();
		/// <summary>
		/// 材质类型和材质
		/// </summary>
		private Dictionary<string, Material> materialDic = new Dictionary<string, Material> ();
		/// <summary>
		/// 牌面的预设
		/// </summary>
		private GameObject CardPerfab;
		/// <summary>
		/// 卡牌池数组
		/// </summary>
		private List<GameObject> cardPoolList = new List<GameObject> ();
		/// <summary>
		/// 表情字典
		/// </summary>
		public Dictionary<int, Sprite[]> stickerLib = new Dictionary<int, Sprite[]> ();
		private BattleProxy battleProxy;
        

		private static ResourcesMgr m_instance;

		public static ResourcesMgr Instance {
			get {
				if (m_instance == null) {
					m_instance = new ResourcesMgr ();
				}
				return m_instance;
			}
		}

		public ResourcesMgr ()
		{
			Mesh[] meshs = Resources.LoadAll<Mesh> ("Models");
			for (int i = 0; i < meshs.Length; i++) {
				meshsDic.Add (meshs [i].name, meshs [i]);
			}
			for (int i = 0; i < GlobalData.CardValues.Length; ++i) {
				meshNameDic.Add (GlobalData.CardValues [i], GlobalData.MeshNames [i]);
			}
			CardPerfab = Resources.Load<GameObject> ("Prefab/Battle/Card");
			for (int i = 1; i <= GlobalData.STICKER_NUM; ++i) {
				stickerLib.Add (i, Resources.LoadAll<Sprite> ("Textures/Sticker/sticker" + i));
			}
			Material[] materials = Resources.LoadAll<Material> ("Materials");
			for (int i = 0; i < materials.Length; i++) {
				materialDic.Add (materials [i].name, materials [i]);
			}
		}

		/// <summary>
		/// 根据牌面值获取牌面的网格数据
		/// </summary>
		/// <param name="card">牌面值 GlobalData.CardValues </param>
		/// <returns></returns>
		private Mesh GetCardMesh (int card)
		{
            if (!meshNameDic.ContainsKey(card))
            {
                Debug.Log("fdsafasf");
            }
			string meshName = meshNameDic [card];
			return meshsDic [meshName];
		}

		/// <summary>
		/// 根据名称获取材质
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Material GetMaterialByName (MaterialName name)
		{
			return materialDic [name.ToString ()];
		}

		/// <summary>
		/// 将牌加入到池内
		/// </summary>
		/// <param name="cardObj"></param>
		public void Add2Pool (GameObject cardObj)
		{
            cardObj.layer = GlobalData.OTHER_CARDS;
			if (cardObj.transform.childCount > 0) {
                Transform icon = cardObj.transform.GetChild(0);
                GameObject.Destroy (icon.gameObject);
                icon.parent = null;
			}
			cardObj.transform.DOKill ();
			cardObj.transform.SetParent (null);
			cardObj.SetActive (false);
			cardPoolList.Add (cardObj);
		}

		/// <summary>
		/// 冲牌池内获取牌
		/// </summary>
		/// <param name="cardValue"></param>
		/// <returns></returns>
		public GameObject GetFromPool (int cardValue)
		{
			GameObject addCard;
			battleProxy = ApplicationFacade.Instance.RetrieveProxy (Proxys.BATTLE_PROXY) as BattleProxy;
			if (cardPoolList.Count > 0) {
				addCard = cardPoolList [cardPoolList.Count - 1];
				cardPoolList.RemoveAt (cardPoolList.Count - 1);
				addCard.SetActive (true);
			} else {
				addCard = GameObject.Instantiate (ResourcesMgr.Instance.CardPerfab);
			}
            if (addCard.transform.childCount > 0)
            {
                UnityEngine.Object.Destroy(addCard.transform.GetChild(0).gameObject);
            }
			if (cardValue != 0 && cardValue == battleProxy.treasureCardCode) {
                if (addCard.transform.childCount == 0)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<SpriteRenderer>();
                    go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/UI/treasureCardCode");
                    go.layer = LayerMask.NameToLayer("SelfHandCards");
                    go.transform.SetParent(addCard.transform);
                    go.transform.localPosition = new Vector3(-0.117f, -0.179f, 0.21f);
                    go.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 132f, 47));
                    go.transform.localScale = new Vector3(.8f, .8f, .8f);
                    //Debug.Log("从getfrompool生成精牌");
                }
			}
            if (cardValue != battleProxy.treasureCardCode)
            {
                if (addCard.transform.childCount > 0)
                {
                    for (int j = 0; j < addCard.transform.childCount; j++)
                    {
                        UnityEngine.GameObject.Destroy(addCard.transform.GetChild(j));
                    }
                }
            }
			var meshFilter = addCard.GetComponent<MeshFilter> ();
            if (cardValue != 0)
            {
                meshFilter.mesh = ResourcesMgr.Instance.GetCardMesh(cardValue);
            }			
			meshFilter.mesh.name = meshFilter.mesh.name.Replace (" Instance", "");
           
			return addCard;
		}

		/// <summary>
		/// 设置牌面
		/// </summary>
		/// <param name="addCard"></param>
		/// <param name="cardValue"></param>
		public void SetCardMesh (GameObject addCard, int cardValue)
		{
            //对精牌做处理？？？
			var meshFilter = addCard.GetComponent<MeshFilter> ();
			meshFilter.mesh = ResourcesMgr.Instance.GetCardMesh (cardValue);
			meshFilter.mesh.name = meshFilter.mesh.name.Replace (" Instance", "");
		}

		/// <summary>
		/// 清空牌池
		/// </summary>
		public void ClearPool ()
		{
			cardPoolList = new List<GameObject> ();
		}

        /// <summary>
        /// 重置池内的对象
        /// </summary>
        public void RecoveryAll()
        {
            foreach (GameObject cardObj in cardPoolList)
            {
                cardObj.transform.DOKill();
                cardObj.transform.SetParent(null);
                cardObj.SetActive(false);
            }
        }
    }
}
