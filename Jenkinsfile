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
                echo 'Build step'
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
