import React from 'react'
import { connect } from 'react-redux'
import * as Actions from 'Redux/iNav/Actions/actions'
import * as GlobalActions from 'Redux/Global/Actions/actions' //TODO: change
import INavNodeContainer from 'iNav/Containers/iNavNodeContainer'
import { INode } from 'Redux/iNav/Types'

if (!SERVER) {
  require('iNav/Css/iNav.NodesContainer')  
}

interface IINavNodesContainer {
  nodes: INode[] 
  activeItemId: number
  onClick: ()=>void
}

const INavNodesContainer: React.StatelessComponent<IINavNodesContainer> = ({ nodes, activeItemId, onClick }) => (
  <div className={'iNav__category iNav-category'} onClick={onClick}>
    {nodes.map((node, index) => {
      return <INavNodeContainer key={node.name} {...node} activeItemId={activeItemId} index={index} />
    })}
  </div>
);

// Connect the Component to the store
export default INavNodesContainer