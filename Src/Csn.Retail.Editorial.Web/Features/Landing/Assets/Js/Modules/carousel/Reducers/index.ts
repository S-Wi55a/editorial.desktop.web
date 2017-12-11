// we wrap the reducer to pass init data to it for it to work in ReactJS.NET
export const carouselParentReducer: any = (initState: any = null) => {
    return (state = initState, action: any) => carouselReducer(state, action);
};

const carouselReducer: any = (state: any, action: any) => {
    switch (action) {
    default:
        return state;
    }
};