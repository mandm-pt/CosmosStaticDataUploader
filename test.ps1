Write-Host "test 123"

if ($env:ReleaseNumber){
  Write-Host "##vso[build.updatebuildnumber]$env:ReleaseNumber.$env:Build_BuildId"
  }
else{
  Write-Host "Release Number not exist, build name not changed"
  }
