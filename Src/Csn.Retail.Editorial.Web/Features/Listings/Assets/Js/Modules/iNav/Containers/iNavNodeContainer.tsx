import React from 'react'
import { connect } from 'react-redux'
import INavfacet from 'iNav/Components/iNavFacet'
import INavConfirmCancelBar from 'iNav/Components/iNavConfirmCancelBar'
import { INode } from 'iNav/Types'
import UI from 'ReactReduxUI'
import { Actions, ActionTypes } from 'iNav/Actions/actions'
import { Dispatch } from 'redux';

if (!SERVER) {
  require('iNav/Css/iNav.NodesContainer')  
}
interface IINavNodeContainer extends INode {
  index: number
  activeItemId: number
}

// This is separated for displaying sub lists
const INavNodeContainer: React.StatelessComponent<IINavNodeContainer> = ({ facets, index, activeItemId, name, displayName }) => (
  <div className={['iNav-category__container', activeItemId === index ? 'isActive' : ''].join(' ')}>
    <div className='iNav-category__header'>{displayName}</div>
    <div className={['iNav-category__container-page'].join(' ')}>
      <div className='iNav-category__list-wrapper'>
        <ul className='iNav-category__list'>
          {facets.map((facet) => {
            return <INavfacet key={facet.displayValue} {...facet} aspect={name}/>
          })}
        </ul>
      </div>
    </div>
    <INavConfirmCancelBar index={index}/>
  </div>
);

// Connect the Component to the store
export default INavNodeContainer