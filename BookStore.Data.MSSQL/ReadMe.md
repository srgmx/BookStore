###### Install dotnet ef CLI:
```dotnet tool install --global dotnet-ef --version 5.0.0```

###### Add BookStore migration:
```dotnet ef migrations add CreateBookStoreDb -s BookStore.API -p BookStore.Data.MSSQL -c BookStoreDbContext -o Migrations\BookStore```

###### Remove BookStore migration:
```dotnet ef migrations remove -s BookStore.API -p BookStore.Data.MSSQL -c BookStoreDbContext```

###### Apply BookStore migration:
```dotnet ef database update -s BookStore.API -p BookStore.Data.MSSQL -c BookStoreDbContext```