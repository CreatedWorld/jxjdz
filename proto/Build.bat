@echo off 
echo ��Э�鶨��ô����ProtoFile�ļ�����
.\Builder\protogen -i:.\ProtoFile\BattleMsg.proto -o:..\client\Assets\Scripts\Platform\Model\Battle\BattleMsg.cs
.\Builder\protogen -i:.\ProtoFile\HallMsg.proto -o:..\client\Assets\Scripts\Platform\Model\\Hall\HallMsg.cs
.\Builder\protogen -i:.\ProtoFile\LoginMsg.proto -o:..\client\Assets\Scripts\Platform\Model\Login\LoginMsg.cs
echo �ļ���������C#Script�ļ�����
pause