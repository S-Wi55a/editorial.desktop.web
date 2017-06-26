import React from 'react'
import INavNodesContainer from 'Js/Modules/iNav/Containers/iNavNodesContainer'


//TODO: memonized

const iNav = () => {

    return (
        <div className={'iNav'}>
            <div className="iNav__category-container">
                <INavNodesContainer/>
            </div> 
        </div>
    )    
}

export default iNav

