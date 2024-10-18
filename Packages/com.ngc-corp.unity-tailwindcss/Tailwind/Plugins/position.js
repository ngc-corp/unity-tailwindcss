plugin(function ({ addUtilities }) {
  const positionUtilities = {
    '.absolute': { position: 'absolute' },
    '.relative': { position: 'relative' },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(positionUtilities, ['responsive', 'hover', 'focus', 'active']);
})
