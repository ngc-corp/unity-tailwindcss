plugin(function ({ addUtilities }) {
  const backgroundImageUtilities = {
    '.bg-none': { 'background-image': 'none' },
  };

  // Add the utilities (responsive, hover, focus, etc.)
  addUtilities(backgroundImageUtilities, ['responsive', 'hover', 'focus', 'active']);
})
