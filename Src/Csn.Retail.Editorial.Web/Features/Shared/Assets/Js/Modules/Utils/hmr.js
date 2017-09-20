export const HMR = (path,fn)=>{
    if (module.hot) {
        module.hot.accept(path,fn)
    }
}