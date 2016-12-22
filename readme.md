# Retail Editorial Website

## How to setup

- Download the repo
- Restore nuget packages
- Run "npm install" from web directory
- Click "build.bat" to build the solution / or use visual studio
- Run `npm run webpack` to build frontend assets
___

## Development

#### Running IIS
- Add `http://redesign.editorial.csdev.com.au/` to bindings
- Run `dev-watch-with-dev-server` to compile frontend assets and enable continuous development
- Open browser at [http://localhost:8080/editorial](http://localhost:8080/editorial)

##### Supports
- SASS compiling
- CSS injecting
- HMR (Hot Module Replacement) (Make sure your code supports [this.](https://webpack.github.io/docs/hot-module-replacement.html#api))

#### Running IIS Express
- Run `npm run dev-watch` to compile frontend assets and enable continuous development 
- Use **Visual Studio Debugging** which should open at [http://localhost:5050/editorial](http://localhost:5050/editorial)


*OR*


- Click `start-web.bat` under src folder. This will start your website using iisexpress against url [http://localhost:5050/editorial](http://localhost:5050/editorial)

##### Supports
- SASS compiling
- Use **Browser Link** for CSS injecting


___