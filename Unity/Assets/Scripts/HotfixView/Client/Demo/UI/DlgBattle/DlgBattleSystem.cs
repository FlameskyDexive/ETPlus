﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[EntitySystemOf(typeof(DlgBattle))]
	[FriendOf(typeof(DlgBattle))]
	[FriendOf(typeof(SkillComponent))]
	public static partial class DlgBattleSystem
	{

		public static void RegisterUIEvent(this DlgBattle self)
        {
            self.View.EBtnSkill1Button.AddListener(self.Root(), () =>
            {
                self.OnClickSkill1();
            });
            self.View.EBtnSkill2Button.AddListener(self.Root(), () =>
            {
                self.OnClickSkill2();
            });
            self.View.E_JoystickJoystick.OnValueChanged.AddListener(self.OnPressJoystick);
            self.View.E_JoystickJoystick.OnPointerUp.AddListener(self.OnUpJoystick);

        }

        public static void ShowWindow(this DlgBattle self, Entity contextData = null)
		{
		}

        public static void RefreshSkillView(this DlgBattle self)
        {
            //刷新技能显示
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.Scene());
            Skill skill1 = null;
            if (unit.GetComponent<SkillComponent>().TryGetSkill(ESkillAbstractType.ActiveSkill, 0, out skill1))
            {
                self.Skill1 = skill1;
                self.InitSkill(self.Skill1, self.View.EIconSkill1Image);
            }
            Skill skill2 = null;
            if (unit.GetComponent<SkillComponent>().TryGetSkill(ESkillAbstractType.ActiveSkill, 1, out skill2))
            {
                self.Skill2 = skill2;
                self.InitSkill(self.Skill2, self.View.EIconSkill2Image);
            }
        }
        private static void InitSkill(this DlgBattle self, Skill skill, Image imgIcon)
        {
            if (skill == null)
                return;
            //初始化设置技能icon等数据
        }

        private static void SetSkillCD(this DlgBattle self, Skill skill, Button btn, Image imgCD, Text cdText, Image imgCover)
        {
            bool isInCd = skill.IsInCd();
            imgCD.gameObject.SetActive(isInCd);
            imgCover.gameObject.SetActive(isInCd);
            cdText.gameObject.SetActive(isInCd);
            if (isInCd)
            {
                cdText.text = $"{(skill.CurrentCD / 1000f).ToString("f1")}";
                imgCD.fillAmount = skill.CurrentCD / (float)skill.CD;
            }
        }

        public static void OnPressJoystick(this DlgBattle self, Vector2 v)
        {
            if (v == Vector2.zero)
                return;
            self.Scene().GetComponent<OperaComponent>().OnMove(v);
            self.LastJoyPos = v;

        }
        public static void OnUpJoystick(this DlgBattle self, Vector2 v)
        {
            self.Scene().GetComponent<OperaComponent>().StopMove();

        }

        public static void OnClickSkill1(this DlgBattle self)
        {
            Log.Info($"click skil1, {self.Skill1 == null}");
            Skill skill = self.Skill1;
            if (skill == null || skill.IsInCd())
                return;
            self.Scene().GetComponent<OperaComponent>()?.OnClickSkill1();
        }
        public static void OnClickSkill2(this DlgBattle self)
        {
            Log.Info($"click skil2, {self.Skill2 == null}");
            Skill skill = self.Skill2;
            if (skill == null || skill.IsInCd())
                return;
            self.Scene().GetComponent<OperaComponent>()?.OnClickSkill2();
        }

        [EntitySystem]
        public static void Awake(this DlgBattle self)
        {
            
        }
        
        [EntitySystem]
        public static void Update(this DlgBattle self)
        {
            if (self.Skill1 != null)
            {
                self.SetSkillCD(self.Skill1, self.View.EBtnSkill1Button, self.View.EIMgCD1Image, self.View.ETextSkill1Text, self.View.EImgMask1Image);
            }
            if (self.Skill2 != null)
            {
                self.SetSkillCD(self.Skill2, self.View.EBtnSkill2Button, self.View.EIMgCD2Image, self.View.ETextSkill2Text, self.View.EImgMask2Image);
            }

        }


    }
}
