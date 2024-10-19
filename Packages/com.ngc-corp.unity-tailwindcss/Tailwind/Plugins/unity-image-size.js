plugin(function ({ addUtilities }) {
  const newUtilities = {
    '.stretch-to-fill': {
      '--unity-image-size': 'stretch-to-fill',
    },
    '.scale-and-crop': {
      '--unity-image-size': 'scale-and-crop',
    },
    '.scale-to-fit': {
      '--unity-image-size': 'scale-to-fit',
    },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(newUtilities, ['responsive', 'hover', 'focus', 'active']);
})
