pipeline {
agent {
  label 'Agent_win'
}

    stages {
        stage('Build') {
            steps {
                git 'https://github.com/nonafk/CQRS_Mediator_Dapper_Api.git'
                bat "dotnet restore CQRS_Mediator_Dapper_Api.sln"
                bat "dotnet build"
                bat "dotnet publish"
            }
        }
        stage('Test') {
            steps {
                echo 'Testing..'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploy..'
            }
        }
    }
}
