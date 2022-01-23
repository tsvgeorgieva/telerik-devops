# Telerik DevOps final project
## Task
This project was developed as a final project for the Telerik Academy Upskill DevOps training. The task is to build a complete automated software delivery process with pipelines using at least 5 of the topics of the course.
- The pipeline starts with git repository
- The solution is T-shaped or E-shaped – where we have a nice working horizontal + at least one deep dive vertical
- Where possible the solution is as code
- Documentation is part of the solution
- Use any tool you like

## Overview
Most of the workflow is automated via Github Actions and can be seen in the /workflows folder of this repo. There are 2 main workflows:
 - release.yml - Running on each push to the main branch
 - pull-request.yml - Running on each pull request to the main branch (opened, reopened, push)

The rest of the workflows are [reusable workflows](https://docs.github.com/en/actions/using-workflows/reusing-workflows) in order to not duplicate code between the PR and Release workflows. 

The PR process is as follows:
 - Application code is submitted as PR
 - Compile & Test
 - SAST with SonarCloud
 - SCA with Snyk

The release process is as follows:
 - Application code is committed to main branch
 - Compile & Test
 - SAST with SonarCloud
 - SCA with Snyk
 - Semantic versioning
 - Build docker image
 - Upload image to Azure Container Registry
 - Tag commit in git
 - Deploy to Azure Kubernetes

![image](https://user-images.githubusercontent.com/7611932/150698724-e1f0f1a7-13f1-4a2a-ac37-b1241b4f0e1e.png)


### Application code
The app is a very simple .NET Core 6 Web API app with one controller - Books. It has Swagger API documentation that is not exposed publicly. Docker support is added.

### Compile & Test
Automated unit tests are run with the dotnet test command. This job also builds the code, so any compile errors will be seen here.

### SAST with SonarCloud
[SonarCloud](https://sonarcloud.io/) is used as a Static Application Security Testing tool, as well as Code Quality checker. In PRs, it will leave comments with the results of the check.

![image](https://user-images.githubusercontent.com/7611932/150697937-e3d6e813-f95b-4141-b006-3b484d15162a.png)

### SCA with Snyk
[Snyk](https://snyk.io/) is used as a Software composition analysis tool to detect vulnerabilities in dependencies. It is set up to run on each PR, as well as daily against the main branch.
It is also integrated with Azure Container Registry in order to scan the docker images that might be deployed to production.

![image](https://user-images.githubusercontent.com/7611932/150697983-a189357d-e917-467f-8341-455a55b5ad0f.png)


### Semantic versioning
In order to follow [Semantic versioning](https://semver.org/) guidelines, I'm using a [github action](https://github.com/PaulHatch/semantic-version). It uses the existing git tags on the main branch, as well as the latest commit message in order to generate a new version number. If the commit contains the text `[Major]` or `[Minor]`, it will increase the corresponding number in the version. Otherwise, the Patch part will be increased. Although this job relies on git tags, it does not automatically push those to the repo. That is done only after the docker image is pushed to the registry.

### Build docker image
The app is built using the Dockerfile.

### Upload image to Azure Container Registry (ACR)
I've created the ACR using the Azure portal. The credentials are stored in github secrets as REGISTRY_USERNAME and REGISTRY_PASSWORD.

### Deploy to Azure Kubernetes Service
I've created the ACR using the Azure portal and Azure CLI. A Helm chart is also used to deploy the application in k8s. You can check out the hosted app [here](http://library-app.c15239172a4945c1a2a1.northeurope.aksapp.io/api/books).

![image](https://user-images.githubusercontent.com/7611932/150698335-ffb0b10e-9bfb-498f-a9ac-9fc602d4834e.png)

## Future improvements
 - add test coverage and add results to SonarCloud
 - add github action for Snyk job
 - automatic versioning of Helm chart
 - infrastructure as code (Terraform)
 - adding metrics and logging for the application
 - Canary/Blue-Green deployment strategy

