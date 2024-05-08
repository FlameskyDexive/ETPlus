using System.Collections.Generic;
using System.Numerics;
using TrueSync;
using Unity.Mathematics;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    [FriendOf(typeof (StateSyncRoomServerComponent))]
    public class C2Room_StateSyncChangeSceneFinishHandler : MessageHandler<Scene, C2Room_StateSyncChangeSceneFinish>
    {
        protected override async ETTask Run(Scene root, C2Room_StateSyncChangeSceneFinish message)
        {
            StateSyncRoom room = root.GetComponent<StateSyncRoom>();
            StateSyncRoomServerComponent roomServerComponent = room.GetComponent<StateSyncRoomServerComponent>();
            StateSyncRoomPlayer roomPlayer = roomServerComponent.GetChild<StateSyncRoomPlayer>(message.PlayerId);
            roomPlayer.Progress = 100;
            
            if (!roomServerComponent.IsAllPlayerProgress100())
            {
                return;
            }
            
            await room.Fiber.Root.GetComponent<TimerComponent>().WaitAsync(1000);

            Room2C_StateSyncStart room2CStart = Room2C_StateSyncStart.Create();
            room2CStart.StartTime = TimeInfo.Instance.ServerFrameTime();
            foreach (StateSyncRoomPlayer rp in roomServerComponent.Children.Values)
            {
                UnitInfo unitInfo = UnitInfo.Create();
                unitInfo.UnitId = rp.Id;
                unitInfo.Position = new float3(RandomGenerator.RandomNumber(-3, 3), 0, RandomGenerator.RandomNumber(-3, 3));
                unitInfo.Forward = new float3(0, 0, 1);
                //��ʼ����ɫ���ԣ����֣�ͷ��id����Ӫ��
                // unitInfo.KV
                room2CStart.UnitInfo.Add(unitInfo);
            }

            room.Init(room2CStart.UnitInfo, room2CStart.StartTime);


            StateSyncRoomMessageHelper.BroadCast(room, room2CStart);
        }
    }
}