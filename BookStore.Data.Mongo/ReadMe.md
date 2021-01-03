###### Run local MongoDB replica set
```mongod --port 27017 --dbpath C:/data/db --replSet rs0 --bind_ip localhost```
###### Initialization is required only once
```mongo rs.initiate()```