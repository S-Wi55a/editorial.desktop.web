# Retail Editorial Website

## How to setup

- Download the repo
- Restore nuget packages
- Run `npm install` from web directory
- Run `npm run build` to build frontend assets
___

## Development

#### Running IIS
- Add `TENANT.editorial.csdev.com.au` to bindings (repalce TENANT with tenantt ie. carsales, bikesales, etc)
- Run `SET TENANT=tenant && npm run dev` to compile frontend assets and enable continuous development
- Remeber to unset global process.env.VAR by `SET TENANT=`
- Open browser at [http://localhost:8080/editorial](http://localhost:8080/editorial)

##### Supports
- SASS compiling
- CSS injecting
- HMR (Hot Module Replacement) (Make sure your code supports [this.](https://webpack.github.io/docs/hot-module-replacement.html#api))

#### Running IIS Express
- Run `npm run dev-watch` to compile frontend assets and enable continuous development
- Use **Visual Studio Debugging** which should open at [http://localhost:5050/editorial](http://localhost:5050/editorial)

___

## Production 

- Run `npm run build-release` to build frontend assets