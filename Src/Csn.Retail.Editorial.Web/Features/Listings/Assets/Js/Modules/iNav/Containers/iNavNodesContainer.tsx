import React from 'react'
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

const INavNodesContainer: React.StatelessComponent<IINavNodesContainer> = ({ nodes, activeItemId }) => (
  <div className={'iNav__category iNav-category'}>
    {nodes.map((node, index) => {
      return <INavNodeContainer key={node.name} {...node} activeItemId={activeItemId} index={index} />
    })}
  </div>
);

// Connect the Component to the store
export default INavNodesContainer