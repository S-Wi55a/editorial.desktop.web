import React from 'react'
import INavMenuHeaderItem from 'iNav/Components/iNavMenuHeaderItem'

if (!SERVER) {
  require('iNav/Css/iNav.MenuHeader.scss')  
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

