import React from 'react'
import INavMenuHeaderItem from 'Js/Modules/iNav/Components/iNavMenuHeaderItem'

if (!SERVER) {
  require('Js/Modules/iNav/css/iNav.MenuHeader.scss')  
}

const INavMenuHeader = ({nodes}) => { 
  
  return (
    <div className={['iNav__menu-header'].join(' ')}>
      {nodes.map((node, index) => {
        return <INavMenuHeaderItem key={index} node={node} index={index} />
      })}
    </div>
  )    
}

export default INavMenuHeader

