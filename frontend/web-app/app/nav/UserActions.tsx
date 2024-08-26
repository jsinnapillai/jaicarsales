'use client'

import { useParamsStore } from '@/hooks/useParamStore'
import { Button, Dropdown, DropdownDivider, DropdownItem } from 'flowbite-react'
import { User } from 'next-auth'
import { signOut } from 'next-auth/react'
import Link from 'next/link'
import { usePathname, useRouter } from 'next/navigation'
 
import React from 'react'
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from 'react-icons/ai'
import { HiCog, HiUser } from 'react-icons/hi'

type Props = {
  user: User
}

export default function UserActions({user}:Props) {
  
  const router = useRouter()
  const pathname = usePathname()
  const setParams = useParamsStore(state => state.setParams)

const setWinner = () => { 
  setParams({
    winner: user.username, seller: undefined
  })
  if(pathname! == "/")
  {
    router.push('/');
  }
}


const setSeller = () => { 
  setParams({
    winner: undefined, seller: user.username
  })
  if(pathname! == "/")
  {
    router.push('/');
  }
}


  return (
   
    <Dropdown inline label={`Welcome ${user.name}`}>

      <DropdownItem icon={HiUser} onClick={setSeller}>
      My Auctions
      </DropdownItem>

      <DropdownItem icon={AiFillTrophy} onClick={setWinner}>
        Auctions Won
      </DropdownItem>

      <DropdownItem icon={AiFillCar}>
        <Link href="/auctions/create">Sell my Car</Link>
      </DropdownItem>

      <DropdownItem icon={HiCog}>
        <Link href="/session">Session (Dev only)</Link>
      </DropdownItem>
    <DropdownDivider/>
      <DropdownItem icon={AiOutlineLogout} onClick={() => signOut({callbackUrl:'/'})}>SignOut
      </DropdownItem>

    </Dropdown>
  )
}
