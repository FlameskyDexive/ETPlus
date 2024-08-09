﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static Unit Create(Scene scene, long id, EUnitType unitType)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case EUnitType.Player:
                {
                    Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    unit.AddComponent<PlayerMoveComponent>();
			
                    NumericComponent numericComponent = unit.AddComponent<NumericComponent>();
                    numericComponent.Set(NumericType.Speed, 5f); // 速度是5米每秒
                    numericComponent.Set(NumericType.AOI, 15000); // 视野15米
                    numericComponent.SetNoEvent(NumericType.MaxHp, unit.Config().Weight);
                    numericComponent.SetNoEvent(NumericType.Hp, unit.Config().Weight);
                    numericComponent.SetNoEvent(NumericType.Attack, 10);//默认攻击力10

                    unitComponent.Add(unit);
                    // 加入aoi
                    unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
                    
                    unit.AddComponent<SkillComponent, List<int>>(new List<int>(){ 1001, 1002 });
                    return unit;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }

        public static Unit CreateBullet(Scene scene, long id, Skill ownerSkill, int config, List<int> bulletData)
        {
            Log.Info($"create bullet");
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit owner = ownerSkill.Unit;
            Unit bullet = unitComponent.AddChildWithId<Unit, int>(id, config);
            MoveComponent moveComponent = bullet.AddComponent<MoveComponent>();
            bullet.Position = owner.Position;
            bullet.Forward = owner.Forward;
            bullet.AddComponent<AOIEntity, int, float3>(15 * 1000, bullet.Position);
            bullet.AddComponent<CollisionComponent>().AddCollider(EColliderType.Circle, Vector2.One * 0.2f, Vector2.Zero, true, bullet);
            bullet.AddComponent<BulletComponent>().Init(ownerSkill, owner);
            NumericComponent numericComponent = bullet.AddComponent<NumericComponent>();
            numericComponent.Set(NumericType.Speed, 10f); // 速度是10米每秒
            numericComponent.Set(NumericType.AOI, 15000); // 视野15米
            //子弹暂时1血量，击中穿透
            numericComponent.SetNoEvent(NumericType.MaxHp, 1);
            numericComponent.SetNoEvent(NumericType.Hp, 1);
            //测试子弹1s后销毁
            float3 targetPoint = bullet.Position + bullet.Forward * numericComponent.GetAsFloat(NumericType.Speed) * 0.6f;
            List<float3> paths = new List<float3>();
            paths.Add(bullet.Position);
            paths.Add(targetPoint);
            moveComponent.MoveToAsync(paths, numericComponent.GetAsFloat(NumericType.Speed)).Coroutine();

            return bullet;
        }
    }
}