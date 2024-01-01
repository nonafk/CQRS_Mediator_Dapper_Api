pipeline {
agent {
  label 'Agent_win'
}

    stages {
        
         stage('checkout') {
            steps {
                git 'https://github.com/nonafk/CQRS_Mediator_Dapper_Api.git'
            }
        }
        
        stage('Build') {
            steps {
                script {
                    bat 'dotnet build CQRS_Mediator_Dapper_Api.sln -c Release'
                }
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
