@echo off 
echo ��Э�鶨��ô����ProtoFile�ļ�����
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/LoginMsg.proto
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/HallMsg.proto
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/BattleMsg.proto
echo �ļ���������\work\server\server\server-trunk\CoreService\src\main\java�ļ�����
pause