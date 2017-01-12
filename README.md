## Synopsis

These are the resources for my "Don't Write Secure Code" talk. This repo contains the slide deck, full speaker notes, and two Visual Studio projects containing demo code.

## Demo Project - "SecureFrameworkDemo"

This is the main demo app. You should be able to open it up in VS 2015+ and run it. 

By default, it will create a database in LocalDb and will populate it with all necessary test data. Everything will work fine EXCEPT the SQL Server 2016 Row Level Security stuff.

To test out RLS, you need to:

1) Edit web.config and set the EnableRowLevelSecurity key to "true"
2) Edit the connection-strings.config file and enter a connection string for a SQL Server 2016 database

To re-do the initial setup, open a Nuget package manager console and type "Update-Database".

## Demo Project - "CSRFAttacker"

This is a quick and dirty app for performing CSRF attacks against the main demo app, for the purposes of testing and verifying the CSRF defense code.

## License

Happily licensed under the WTFPL. I've learned so much from the open web in my career, couldn't be happier about contributing to it.

http://www.wtfpl.net/