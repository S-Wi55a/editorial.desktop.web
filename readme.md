# Retail Editorial Website

## How to setup

- Download the repo
- Restore nuget packages
- Run `npm install` from web directory
- Run `npm run webpack` to build frontend assets
___

## Development

#### Running IIS
- Add `TENET.editorial.csdev.com.au` to bindings (repalce TENET with tenet ie. carsales, bikesales, etc)
- Run `dev-watch-with-dev-server  -- tenet=TENET` to compile frontend assets and enable continuous development  (*Tenet defaults to carsales*)
- Open browser at [http://localhost:8080/editorial](http://localhost:8080/editorial)

##### Supports
- SASS compiling
- CSS injecting
- HMR (Hot Module Replacement) (Make sure your code supports [this.](https://webpack.github.io/docs/hot-module-replacement.html#api))

#### Running IIS Express
- Run `npm run dev-watch  -- tenet=TENET` to compile frontend assets and enable continuous development  (*Tenet defaults to carsales*)
- Use **Visual Studio Debugging** which should open at [http://localhost:5050/editorial](http://localhost:5050/editorial)


*OR*


- Click `start-web.bat` under src folder. This will start your website using iisexpress against url [http://localhost:5050/editorial](http://localhost:5050/editorial)

##### Supports
- SASS compiling
- Use **Browser Link** for CSS injecting


___


## Production

- `npm run build-release -- awsBucketBasePath=DIRECTORY --awsAccessKey=AWS_ACCESS_KEY --awsSecret=AWS_SECRET`
___
