@echo off 
echo 把协议定义好存放在ProtoFile文件夹中
.\Builder\protogen -i:.\ProtoFile\BattleMsg.proto -o:..\client\Assets\Scripts\Platform\Model\Battle\BattleMsg.cs
.\Builder\protogen -i:.\ProtoFile\HallMsg.proto -o:..\client\Assets\Scripts\Platform\Model\\Hall\HallMsg.cs
.\Builder\protogen -i:.\ProtoFile\LoginMsg.proto -o:..\client\Assets\Scripts\Platform\Model\Login\LoginMsg.cs
echo 文件已生成在C#Script文件夹中
pause