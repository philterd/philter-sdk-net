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
        sh "dotnet restore"
        sh "dotnet build -c Release"
        sh "dotnet pack -c Release --version-suffix ${BUILD_NUMBER}"
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
        sh "dotnet nuget push ${env.WORKSPACE}/bin/Release/philter-sdk-net.1.0.0.nupkg --api-key $NUGET_KEY --source $NUGET_SOURCE"
      }
    }
  }
}
