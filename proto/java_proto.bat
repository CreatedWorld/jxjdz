@echo off 
echo 把协议定义好存放在ProtoFile文件夹中
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/LoginMsg.proto
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/HallMsg.proto
protoc --java_out=\SVNCheckout\jxjdz\server\Common\src\main\java ./ProtoFile/BattleMsg.proto
echo 文件已生成在\work\server\server\server-trunk\CoreService\src\main\java文件夹中
pause
