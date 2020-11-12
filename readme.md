# Notification Service - RabbitMQ, Sendgrid, Slack.
The idea here is to consume events from RabbitMQ and send email via Sendgrid or add messages to Slack channel. Let's say, there are some business critical situations, when they occure, some events are triggered to a web endpoint. Those events are then stored in RabbitMQ (covered in my other repository, please check here https://github.com/mail4hafij/rabbit_event_stream). Now, in this solution we want to consume those events from RabbitMQ so that we can notify people by email and send messages in Slack channel. 

Techonology stack: Docker-compose, .Net core 2.0, C#, RabbitMQ client.

How to run locally:
This project can be debugged in Visual Studio given you have the docker support enabled. 
https://docs.microsoft.com/en-us/dotnet/standard/containerized-lifecycle-architecture/design-develop-containerized-apps/visual-studio-tools-for-docker

1. Checkout the repository https://github.com/mail4hafij/rabbit_event_stream and run.
  
  ``` docker-compose up --build ```

2. Start the service (notification_service) with docker-compose. This will attach the container to the rabbit_event_stream network. 

  ``` docker-compose up --build ```   

or if you don't want to use docker-compose then  

2(a). First build it from the docker file (Dockerfile.local): 
  
  ``` docker build -f Dockerfile.local . -t notification_service_src ```  

2(b). Then run it with the following mentioned paremeters. This will attach the container to the rabbit_event_stream network:
  
  ``` docker run --name any_name_container --network=rabbit_event_stream notification_service_src ``` 

Debug:
1. Open notification_service solution in Visual Studio (in order to start fresh, delete the .vs folder and make sure the docker host is running when opening the solution. Then set docker-compose as startup project which will spin up the container). Hit the run button.
2. Go to the rabbit_event_stream/scenarios and run 'dummy_events.js'
