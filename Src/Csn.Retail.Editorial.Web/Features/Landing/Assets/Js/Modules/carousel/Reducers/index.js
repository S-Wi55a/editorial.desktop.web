

//We wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const carouselParentReducer = (initState = null) => {
    return (state = initState, action) => carouselReducer(state, action)
}

const carouselReducer = (state, action) => {
    switch (action) {
    default:
        return state;
    }
}