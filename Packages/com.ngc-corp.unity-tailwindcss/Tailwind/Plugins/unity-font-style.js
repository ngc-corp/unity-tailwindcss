plugin(function ({ addUtilities }) {
  const newUtilities = {
    '.normal': {
      '-unity-font-style': 'normal',
    },
    '.italic': {
      '-unity-font-style': 'italic',
    },
    '.bold': {
      '-unity-font-style': 'bold',
    },
    '.bold-and-italic': {
      '-unity-font-style': 'bold-and-italic',
    },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(newUtilities, ['responsive', 'hover', 'focus', 'active']);
})
