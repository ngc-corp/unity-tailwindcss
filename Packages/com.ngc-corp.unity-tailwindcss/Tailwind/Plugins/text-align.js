plugin(function ({ addUtilities }) {
  const newUtilities = {
    '.text-upper-left': {
      '-unity-text-align': 'upper-left',
    },
    '.text-middle-left': {
      '-unity-text-align': 'middle-left',
    },
    '.text-lower-left': {
      '-unity-text-align': 'lower-left',
    },
    '.text-upper-center': {
      '-unity-text-align': 'upper-center',
    },
    '.text-middle-center': {
      '-unity-text-align': 'middle-center',
    },
    '.text-lower-center': {
      '-unity-text-align': 'lower-center',
    },
    '.text-upper-right': {
      '-unity-text-align': 'upper-right',
    },
    '.text-middle-right': {
      '-unity-text-align': 'middle-right',
    },
    '.text-lower-right': {
      '-unity-text-align': 'lower-right',
    },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(newUtilities, ['responsive', 'hover', 'focus', 'active']);
})
