# Internal notification service RabbitMQ, Sendgrid, Slack.
The idea here is to consume events from RabbitMQ and send email via Sendgrid or add messages to Slack channel. Let's say, there are some business critical situations, when they occure, some events are triggered to a web endpoint. Those events are then stored in RabbitMQ (convered in my other repository, please check here https://github.com/mail4hafij/rabbit_event_stream). Now, in this solution we want to consume those events from RabbitMQ so that we can notify people by email and send messages in Slack channel. 

Techonology stack: Docker-compose, .Net core 2.0, C#, RabbitMQ client.

How to run locally:
This project can be debugged in Visual Studio given you have the docker support enabled. 
https://docs.microsoft.com/en-us/dotnet/standard/containerized-lifecycle-architecture/design-develop-containerized-apps/visual-studio-tools-for-docker

1. src/Dockerfile - It builds the src.
2. docker-compose.yml - It builds the servcie in 'dev' mode in the same network as rabbit_event_stream



1. Checkout the repository https://github.com/mail4hafij/rabbit_event_stream and run.
2. Open internal_notification_service solution in Visual Studio (in order to start fresh, delete the .vs folder and make sure the docker host is running when opening the solution. 
Then set docker-compose as startup project which will spin up the container). Hit the run button.
3. Go to the rabbit_event_stream/scenarios and run 'dummy_events.js'

