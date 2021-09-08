# Ecommerce microservices solution
## In order to build and push individual images to the dockerhub do such steps:
1. Copy individual Docker file for needed service to the solution directory
2. Navigate to the solution directory and run cmd or powershell with such commands:
```sh
docker build -t /ecommerceapi<service_name> .
docker run <your_username>/ecommerceapi<service_name>
docker push <your_username>/ecommerceapi<service_name>
```
3. Remove Dockerfile from solution folder or replace with one for the next service to build image for