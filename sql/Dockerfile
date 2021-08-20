# Elegimos la etiqueta exacta (no 'latest'), para estar seguros de que la nueva versión no romperá la creación de la imagen
FROM mcr.microsoft.com/mssql/server

# Usando un usuario con privilegios
USER root

# Crear directorio de aplicaciones
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Copiar scripts de inicialización
COPY . /usr/src/app

# Conceder permisos para que el script de inicialización de ejecución sea ejecutable
RUN chmod +x /usr/src/app/run-initialization.sh

# Establecer variables de entorno, para no tener que escribirlas con el comando docker run
# Nota: asegúrese de que su contraseña coincida con lo que está en el script de inicialización de ejecución
ENV SA_PASSWORD Sql2021@
ENV ACCEPT_EULA Y

# Exponer el puerto 1433 en caso de acceso desde otro contenedor
#EXPOSE 1433

# Ejecutar Microsoft SQl Server y script de inicialización (al mismo tiempo)
# Nota: Si desea iniciar MsSQL sólo (sin script de inicialización) puede comentar abajo la línea de salida, 
# se tomará la entrada CMD de la imagen base
CMD /bin/bash ./entrypoint.sh
