import React from 'react'
import { Transition }  from 'react-transition-group'

const transitionStyles = {
  entering: { opacity: 1 },
  entered:  { opacity: 1 },
};

export const Fade = ({ in: inProp, children, duration, startingOpacity = 0 }) => (
  <Transition in={inProp} timeout={duration || 0} startingOpacity={startingOpacity}>
    {(state) => (
      <span style={{
        ...{
            transition: `opacity ${duration}ms ease-in-out`,
            opacity: startingOpacity,
          },
        ...transitionStyles[state]
      }}>
        {children}
      </span>
    )}
  </Transition>
);