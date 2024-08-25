'use client'

import Image from 'next/image'
import React, { useState } from 'react'

type Props = {
    imageUrl: string
}

export default function CarImages({imageUrl}:Props) {
    const [isLoading,setIsLoading] = useState(true);

  return (
    <Image
    src={imageUrl}
    alt={'image of car'}
    fill
    className={`object-cover group-hover:opacity-75 duration-75 ease-in-out 
        ${isLoading ? 'grayscale blur-2xl scale-110' :'grayscale-0 blur-0 scale-100' }`}
    priority
    sizes="(max-widht:760px) 100vw, (max-width:1200px) 50vw  25vw " onLoad={() => {setIsLoading(false)}}
  />
  )
}
