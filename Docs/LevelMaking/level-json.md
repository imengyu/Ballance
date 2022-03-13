```json
{
  //关卡名字，这是给玩家看的，不是加载和打包时的名字
  "name": "关卡名字",
  //关卡作者
  "author": "关卡作者",
  //关卡版本，例如1.0
  "version": "1.0",
  //关卡介绍文字，在256字以内
  "introduction": "关卡介绍文字，在256字以内",
  //关卡介绍页面的自定义链接，可以设置到您的博客或者github
  "url": "https://yourwebsite.com",
  //是否允许使用关卡查看器预览此关卡（TODO）
  "allowPreview": false,
  //如果本关卡依赖某些模组，可以在这里声明，如果没有，可以为空数组 []
  "requiredPackages": [
    {
      "name": "com.your.mod.name", //依赖模组的包名
      "minVersion": 0 //依赖模组最低版本
    }
  ],
  //关卡主信息
  "level": {
    //初始的球类型
    //内置有三种球
    //BallWood 木球
    //BallStone 石球
    //BallPaper 纸球
    //模组也可以添加自定义球，如果要设置初始球为自定义球，名字必须和注册时一样
    "firstBall": "BallWood",
    //关卡的过关分数
    "levelScore": 100,
    //关卡开始时的时间点
    "startPoint": 1000,
    ///关卡开始时的生命
    "startLife": 3,
    //关卡音乐预设
    //原版有1-5 五个音乐预设
    //为0则没有背景音乐
    //你也可以通过设置customMusicTheme来自定义音乐预设，如果选择使用
    //自定义预设，则这里填写 customMusicTheme.id 字段。
    "musicTheme": 1,
    //天空盒预设
    //原版有 A-L 12个预设
    //如果填写custom，则使用customSkyBox中定义的天空盒。
    "skyBox": "A",
    //云层, 支持两种，需要在您的关卡中先添加占位符
    //SkyLayer  平面云层
    //SkyVoterx 漩涡云层
    "skyLayer": "SkyLayer",
    //指定当前关卡下一关，如果为空，则过关之后不会显示下一关按扭
    "nextLevel": "Levelx",
    //自定义背景音乐预设
    "customMusicTheme": {
      "id": 8, //ID
      "baseInterval": 5, //最低间隔，单位秒
      "maxInterval": 30, //最高间隔，单位秒
      "atmoInterval": 6, //副音乐最低间隔，单位秒
      "atmoMaxInterval": 15, //副音乐最高间隔，单位秒
      "musics": [
        //主音乐，音乐必须放在当前关卡文件夹下一起打包。路径不需要完整，文件名就可以。
        "xxx.wav",
        "yyy.wav"
      ],
      "atmos": [
        //副音乐
        "xxx.wav",
        "yyy.wav"
      ]
    },
    //自定义天空盒预设，B后 F 前 L 左 R 右 T 上 D 下
    "customSkyBox": {
      //图片必须放在当前关卡文件夹下一起打包。
      "B": "BackResourceName.jpg",
      "F": "FrontResourceName.jpg",
      "L": "LeftResourceName.jpg",
      "R": "RightResourceName.jpg",
      "T": "TopResourceName.jpg",
      "D": "DownResourceName.jpg"
    },
    //当前关卡的默认高分数据，如果不指定，则使用1-11关的默认数据
    "defaultHighscoreData": [
      { "name": "Mr. Default", "score": 7000, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 6600, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 6200, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 5800, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 5400, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 5000, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 4600, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 4200, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 3800, "date": "2004/8/8" },
      { "name": "Mr. Default", "score": 3600, "date": "2004/8/8" }
    ],
    //当前关卡的灯光颜色
    "lightColor": "#ffffff",
    //当前关卡的自定义模组事件触发器
    "customModEventName": "",
    //当前关卡节数
    "sectorCount": 2,
    //关卡过关时是否播放UFO动画
    "endWithUFO": false,
    //自动归组，详情见自动归组说明文档
    "autoGroup": false,
    //内置物体名字数据
    "internalObjects": {
      "PS_LevelStart": "PS_FourFlames_01", //开始火焰的占位符名称
      "PE_LevelEnd": "PE_Balloon_01", //结束飞船的占位符名称
      "PC_CheckPoints": { //检查点的占位符名称
        "2": "PC_TwoFlames_01" //这里的索引是从2开始的，2就表示是第二节，因为第一节是开始火焰，所以这里没有1.
      },
      "PR_ResetPoints": {  //出生点的占位符名称
        "1": "PR_Resetpoint_01", //索引是几就表示是第几节
        "2": "PR_Resetpoint_02"
      }
    },
    //小节激活的机关
    //注意，这里的数量必须与sectorCount一致。
    "sectors": {
      "1": [ "objectName", "objectName" ],
      "2": [ "objectName" ]
    },
    //路面信息
    "floors": [
      {
        "name": "Phys_Floors",
        "objects": []
      },
      {
        "name": "Phys_FloorWoods",
        "objects": []
      },
      {
        "name": "Phys_FloorRails",
        "objects": []
      },
      {
        "name": "Phys_FloorStopper",
        "objects": []
      }
    ],
    //死亡碰撞区的信息
    "depthTestCubes": [
      "Quader01",
      "Quader02",
      "Quader03"
    ],
    //机关归组的信息
    "groups": [
      {
        "name": "P_Box",
        "objects": []
      },
      {
        "name": "P_Dome",
        "objects": []
      },
      {
        "name": "P_Ball_Stone",
        "objects": []
      },
      {
        "name": "P_Ball_Paper",
        "objects": []
      },
      {
        "name": "P_Ball_Wood",
        "objects": []
      },
      {
        "name": "P_Extra_Life",
        "objects": []
      },
      {
        "name": "P_Extra_Point",
        "objects": []
      },
      {
        "name": "P_Trafo_Stone",
        "objects": []
      },
      {
        "name": "P_Trafo_Paper",
        "objects": []
      },
      {
        "name": "P_Trafo_Wood",
        "objects": []
      },
      {
        "name": "P_Modul_01",
        "objects": []
      },
      {
        "name": "P_Modul_03",
        "objects": []
      },
      {
        "name": "P_Modul_08",
        "objects": []
      },
      {
        "name": "P_Modul_17",
        "objects": []
      },
      {
        "name": "P_Modul_18",
        "objects": []
      },
      {
        "name": "P_Modul_19",
        "objects": []
      },
      {
        "name": "P_Modul_25",
        "objects": []
      },
      {
        "name": "P_Modul_26",
        "objects": []
      },
      {
        "name": "P_Modul_29",
        "objects": []
      },
      {
        "name": "P_Modul_30",
        "objects": []
      },
      {
        "name": "P_Modul_34",
        "objects": []
      },
      {
        "name": "P_Modul_37",
        "objects": []
      },
      {
        "name": "P_Modul_41",
        "objects": []
      }
    ]
  }
}
```