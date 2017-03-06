To run the application
1) Install visual studio 2017 professional(RC at the time of writing this)
2) Add the following package sources in visual studio under Tools -> Options -> Nuget Package Manager -> Package sources
    a)https://www.myget.org/F/aspnet-contrib/api/v3/index.json
    b)https://dotnet.myget.org/F/aspnetcore-ci-dev/api/v3/index.json
3) Open Quantium.Recruitment.Portal folder in command prompt and run the command "npm install"
4) After the packages are installed, run the command "npm run build:dev"
5) Now you can debug the application in visual studio like any other web application
6) This has Webpack HMR enabled in chrome. So any change you make on client resources will be immediately reflected.(Thanks to asadsahi)

