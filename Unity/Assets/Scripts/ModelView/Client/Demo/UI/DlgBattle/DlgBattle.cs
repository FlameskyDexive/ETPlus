﻿using UnityEngine;

namespace ET.Client
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgBattle :Entity,IAwake,IUILogic,IUpdate
	{

		public DlgBattleViewComponent View { get => this.GetComponent<DlgBattleViewComponent>();}


        public EntityRef<Skill> Skill1 { get; set; }
        public EntityRef<Skill> Skill2 { get; set; }

        public EntityRef<Unit> MyUnit { get; set; }
        
        public Vector2 LastJoyPos { get; set; }

    }
}
