using System;
using System.Collections.Generic;

namespace ET.Server
{

    [FriendOf(typeof(MatchComponent))]
    public static partial class MatchComponentSystem
    {
        public static async ETTask Match(this MatchComponent self, long playerId)
        {
            if (self.waitMatchPlayers.Contains(playerId))
            {
                return;
            }
            
            self.waitMatchPlayers.Add(playerId);

            if (self.waitMatchPlayers.Count < LSConstValue.MatchCount)
            {
                return;
            }
            
            // 申请一个房间
            StartSceneConfig startSceneConfig = RandomGenerator.RandomArray(StartSceneConfigCategory.Instance.Maps);
            Match2Map_GetRoom match2MapGetRoom = Match2Map_GetRoom.Create();
            foreach (long id in self.waitMatchPlayers)
            {
                match2MapGetRoom.PlayerIds.Add(id);
            }
            
            self.waitMatchPlayers.Clear();

            Scene root = self.Root();
            Map2Match_GetRoom map2MatchGetRoom = await root.GetComponent<MessageSender>().Call(
                startSceneConfig.ActorId, match2MapGetRoom) as Map2Match_GetRoom;

            Match2G_NotifyMatchSuccess match2GNotifyMatchSuccess = Match2G_NotifyMatchSuccess.Create();
            match2GNotifyMatchSuccess.ActorId = map2MatchGetRoom.ActorId;
            MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
            
            foreach (long id in match2MapGetRoom.PlayerIds) // 这里发送消息线程不会修改PlayerInfo，所以可以直接使用
            {
                messageLocationSenderComponent.Get(LocationType.Player).Send(id, match2GNotifyMatchSuccess);
                // 等待进入房间的确认消息，如果超时要通知所有玩家退出房间，重新匹配
            }
        }
        public static async ETTask StateSyncMatch(this MatchComponent self, long playerId)
        {
            if (self.waitMatchStateSyncPlayers.Contains(playerId))
            {
                return;
            }
            Scene root = self.Root();

            self.waitMatchStateSyncPlayers.Add(playerId);
            //广播消息给每个玩家，当前匹配到的信息。
            Match2G_StateSyncRefreshMatch refreshMatch = Match2G_StateSyncRefreshMatch.Create();
            //内网通信传playerids给Gate，Gate获取到PlayerId对应的Player信息再广播到客户端
            refreshMatch.PlayerIds.AddRange(self.waitMatchStateSyncPlayers);
            
            
            if (self.waitMatchStateSyncPlayers.Count < ConstValue.StateSyncMatchCount)
            {
                return;
            }

            // 申请一个房间
            StartSceneConfig startSceneConfig = RandomGenerator.RandomArray(StartSceneConfigCategory.Instance.Maps);
            Match2Map_StateSyncGetRoom match2MapGetRoom = Match2Map_StateSyncGetRoom.Create();
            foreach (long id in self.waitMatchStateSyncPlayers)
            {
                match2MapGetRoom.PlayerIds.Add(id);
            }

            self.waitMatchStateSyncPlayers.Clear();

            Map2Match_StateSyncGetRoom map2MatchGetRoom = await root.GetComponent<MessageSender>().Call(
                startSceneConfig.ActorId, match2MapGetRoom) as Map2Match_StateSyncGetRoom;

            Match2G_StateSyncNotifyMatchSuccess match2GNotifyMatchSuccess = new() { ActorId = map2MatchGetRoom.ActorId };
            MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();

            foreach (long id in match2MapGetRoom.PlayerIds) // 这里发送消息线程不会修改PlayerInfo，所以可以直接使用
            {
                messageLocationSenderComponent.Get(LocationType.Player).Send(id, match2GNotifyMatchSuccess);
                // 等待进入房间的确认消息，如果超时要通知所有玩家退出房间，重新匹配
            }
        }
    }

}