# CatsVsDogsBinaryClassification
ASP.NET Core 3.1 Web API using deep learning model generated by [ML .NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet). 
### About 
Web API takes a picture as an input and predicts whether it's a cat or a dog on it.
### Used dataset
Dataset used to train the model can be found [here](https://www.kaggle.com/c/dogs-vs-cats/data)
### Deep learning model
Model used by the app can be found [here](https://github.com/szymenn/CatsVsDogsBinaryClassification/tree/master/CatsVsDogs.Api/MLModel). 

Model was generated by [ModelBuilder](https://github.com/szymenn/CatsVsDogsBinaryClassification/tree/master/ModelBuilder) project using ML .NET
### Client app
Client app that's using this Web API can be found in [this repository](https://github.com/szymenn/cats-vs-dogs-frontend)
### Used technologies
- C# 8.0
- ASP.NET Core 3.1
- ML .NET
- Entity Framework Core 
- PostgreSQL
- Docker
### Run on your computer
Prerequisites:
- Git
- Docker

To run the app simply clone repository using: <br /> 

`git clone https://github.com/szymenn/CatsVsDogsBinaryClassification.git` <br />

Then in main directory build application images using docker compose: <br />

`docker-compose build` <br />

and finally start the app: <br />

`docker-compose up`

### How to use 
Application consists of two endpoints, one for getting prediction history and one for predicting data based on the input picture. <br />

To get prediction history run this using your prefered client (postman, fidler etc.): <br />
`HTTP GET http://localhost:5000/api/prediction`

To get prediction based on input picture run this with picture included within the request: <br />
`HTTP POST http://localhost:5000/api/prediction`


