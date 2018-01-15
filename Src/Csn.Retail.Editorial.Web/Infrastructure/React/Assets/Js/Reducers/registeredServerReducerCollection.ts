import {iNavParentReducer} from "ReactComponents/iNav/Reducers";
import {carouselParentReducer} from "carousel/Reducers";

const Reducers: any = {
    listings: iNavParentReducer,
    carousels: carouselParentReducer
};

export default Reducers;