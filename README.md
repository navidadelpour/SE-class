# SE-class
unity game for software engineering class

these instructions helps you to better use git in your unity projects

how to clone your project from github:

1- create a new folder with the project name

2- clone the repository with these commands in the prject folder

    git init
    git remote add origin https://github.com/navidadelpour/SE-class.git
    git pull origin master


3- open the project in unity

how to complete your code and add features:

foreach changes you want to make please create a new branch and then after finishing your job merge into the master

1- create a branch and switch on it
    
    git branch <new-branch-name>
    git checkout <new-branch-name>

2- commit your changes to the branch for example:

    git commit -m "feature added but there is a bug"
    git commit -m "bug fixed"
    git commit -m "feature added completely"
    
3- switch to your master branch

    git checkout master
    
4- pull the project again 

    git pull origin master

4- merge your branch with master

    git merge <branch-name>

5- handle conflicts (optional)

6- push your changes to the repository

    git push origin master
