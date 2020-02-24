pipeline {
    agent any
    triggers {
        pollSCM 'H/10 * * * *'
    }
    options {
        buildDiscarder(logRotator(numToKeepStr: '3'))
    }
    parameters {
        booleanParam(defaultValue: false, description: 'Publish NuGet package', name: 'isPublish')
    }
    stages {
        stage ('Build') {
          steps {
              sh "./build.sh"
          }
        }
        stage ('Publish') {
          when {
              expression {
                  if (env.isPublish == "true") {
                      return true
                  }
                  return false
              }
          }
          steps {
              sh "dotnet nuget push philter-sdk-net.1.0.0.nupkg -s https://api.nuget.org/v3/index.json"
          }
        }
    }
}
