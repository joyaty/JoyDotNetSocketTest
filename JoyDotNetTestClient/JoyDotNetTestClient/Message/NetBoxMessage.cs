
using System.Text.Json.Serialization;

namespace Joy.Test.Client
{
    public enum NetCmd : int
    {
        CreateGame = 10001,             // 创建房间
        Shot = 10002,                   // 击球
    }

    public class NetBoxMessage
    {
        [JsonInclude]
        public int cmd;
        [JsonInclude]
        public string msg;
        [JsonInclude]
        public int code = 200;            // 错误码，默认200是正确的
        [JsonInclude]
        public string data;
    }

    public class NetRcvCreateGame
    {
        [JsonInclude]
        public int game_mode;
        [JsonInclude]
        public int ballCount = -1;       // 指定球的数量，9球4,6,9， 斯诺克 红球数量
        [JsonInclude]
        public bool isZhuiFenMode = false;      // 是否追分模式
    }

    public class NetRcvShot
    {
        [JsonInclude]
        public long gameId;
        [JsonInclude]
        public int round;               // 回合数
                                        // 打球时，如果指定球可以移动，则打球时指定一个位置放置后再打，这里可以是不同消息或者有标志位
        [JsonInclude]
        public bool needPutBall;
        [JsonInclude]
        public float px;
        [JsonInclude]
        public float py;
        [JsonInclude]
        public float pz;
        [JsonInclude]
        public int playerNextHitBall;              // 本回合玩家打的目标球id，同步模式传递的值不一样
        [JsonInclude]
        public PhysicsShotData hitBallDetail;       // 数据结构一直
    }

    public struct PhysicsShotData
    {
        [JsonInclude]
        public int BallId;      // 打击球的实力id
        /// <summary>
        /// 球杆质量，来源球杆配置表
        /// </summary>
        [JsonInclude]
        public float CueMass;
        /// <summary>
        /// 球杆的最大击球速度，来源球杆配置表
        /// </summary>
        [JsonInclude]
        public float Speed;

        /// <summary>
        /// 拉杆力度，值域:[0, 1]
        /// </summary>
        [JsonInclude]
        public float Power;

        /// <summary>
        /// 撞击方向(与Z轴负向的夹角，单位:角度，值域:[0, 360])
        /// </summary>
        [JsonInclude]
        public float Phi;

        /// <summary>
        /// 撞击高度角(与XZ平面的夹角，单位:角度，值域:[0, 90])
        /// </summary>
        [JsonInclude]
        public float Theta;

        /// <summary>
        /// 撞击点水平偏移[-Radius,Radius]
        /// </summary>
        [JsonInclude]
        public float Horizontal;

        /// <summary>
        /// 撞击点垂直偏移[-Radius,Radius]
        /// </summary>
        [JsonInclude]
        public float Vertical;

        public static PhysicsShotData GetShotData(float power)
        {
            return new PhysicsShotData()
            {
                BallId = 0,
                CueMass = 0.170097f,
                Speed = 3.0f,
                Power = power,
                Theta = 0f,
                Phi = 90f,
                Horizontal = 0f,
                Vertical = 0f,
            };
        }
    }
}