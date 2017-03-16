# QRecruitment phase 1

<p>To run the application</p>
<ol>
    <li> Install visual studio 2017 professional(RC at the time of writing this)</li>
    <li>Add the following package sources in visual studio under Tools -> Options -> Nuget Package Manager -> Package sources</li>
        <ol>
            <li>https://www.myget.org/F/aspnet-contrib/api/v3/index.json</li>
            <li>https://dotnet.myget.org/F/aspnetcore-ci-dev/api/v3/index.json</li>
        </ol>
    <li> Open Quantium.Recruitment.Portal folder in command prompt and run the command "npm install"</li>
    <li> After the packages are installed, run the command "npm run build:dev"</li>
    <li> Now you can debug the application in visual studio like any other web application</li>
    <li> This has Webpack HMR enabled in chrome. So any change you make on client resources will be immediately reflected.(Thanks to asadsahi)</li>
</ol>
<p>Pre requesite To run the Mail Sender Job </p>
 <ol>Run the Scripts_sp.sql in the Recruitment DB (Will try to remove this dependency)</ol>

