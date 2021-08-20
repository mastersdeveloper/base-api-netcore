# Espere para asegurarse de que SQL Server surgió
sleep 90s

# Ejecute el script de instalación para crear la base de datos y el esquema en la base de datos
# Nota: asegúrese de que su contraseña coincida con lo que está en el Dockerfile
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Sql2021@ -d master -i create-database.sql