
@echo off

forfiles /p "D:\Mysql_Backup" /m jfdb_backup_*.sql -d -30 /c "cmd /c del /f @path"

set "Ymd=%date:~0,4%%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%"

"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump" --opt --single-transaction=TRUE --user=root --password=root@jf --host=localhost --protocol=tcp --port=3306 --default-character-set=utf8 --single-transaction=TRUE --routines --events "jfdb" > "D:\Mysql_Backup\jfdb_backup_%Ymd%.sql"


@echo on
