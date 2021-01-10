###### Run local MongoDB replica set
```mongod --port 27017 --dbpath C:/data/db --replSet rs0 --bind_ip localhost```
###### Initialization is required only once
```mongo rs.initiate()```

###### Mongo queries examples for joining collections data
db.Book.aggregate([{"$lookup": {"from": "Author", "localField": "authors._id", "foreignField": "_id", "as": "authors" }}])
db.Book.aggregate([{"$lookup": {"from": "Author", "localField": "authors._id", "foreignField": "_id", "as": "authors" }}, {"$match": {"_id": UUID("60b92eec-8457-4786-bdd4-2470979bbf3c")}}])